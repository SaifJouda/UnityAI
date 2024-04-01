using UnityEngine;
using Unity.Sentis;
using Unity.Sentis.Layers;
using System;
using System.Collections;

using UnityEngine.UI;
public class NumberProcessor : MonoBehaviour
{
    [SerializeField] private ModelAsset modelAsset;
    [SerializeField] private Texture2D inputTexture;
    [SerializeField] private float[] results;

    
    
    Model runtimeModel;
    IWorker worker;
    TensorFloat inputTensor;

    public Text textObject; 
    void Start()
    {
        runtimeModel = ModelLoader.Load(modelAsset);

        string softmaxOutputName = "Softmax_Output";
        
        runtimeModel.AddLayer(new Softmax(softmaxOutputName, runtimeModel.outputs[0]));
        runtimeModel.outputs[0]= softmaxOutputName;

        worker = WorkerFactory.CreateWorker(BackendType.GPUCompute, runtimeModel);

        //ExecuteModel();
    }

    public void ExecuteModel()
    {
        //if something is already running, dispose of it
        inputTensor?.Dispose();

        inputTensor = TextureConverter.ToTensor(inputTexture,28,28,1);
        worker.Execute(inputTensor);

        TensorFloat outputTensor= worker.PeekOutput() as TensorFloat;
        outputTensor.MakeReadable();
        results=outputTensor.ToReadOnlyArray();
        outputTensor.Dispose();
        results=RoundResults(results);
        FindLargestIndex(results);
    }

    private void OnDisable()
    {
        inputTensor?.Dispose();
        worker.Dispose();
    }

    private float[] RoundResults(float[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = (float)Math.Round(numbers[i], 2);
        }
        return numbers;
    }

    void FindLargestIndex(float[] arr)
    {
        int largestIndex = 0;
        float largestValue = arr[0];

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] > largestValue)
            {
                largestValue = arr[i];
                largestIndex = i;
            }
        }

        textObject.text = "Guessed Number: "+largestIndex;
    }

}
