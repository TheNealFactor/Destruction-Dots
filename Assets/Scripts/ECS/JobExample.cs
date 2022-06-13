using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class JobExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Do example!");
        DoExample();
    }

    private void DoExample()
    {

        NativeArray<float> resultArray = new NativeArray<float>(1, Allocator.TempJob);
        // instantiate job and initialize
        SimpleJob myJob = new SimpleJob
        {
            a = 5f,
            result = resultArray
        };

        AnotherJob secondjob = new AnotherJob();
        secondjob.result = resultArray;

        //schedule Job
        JobHandle handle = myJob.Schedule();
        JobHandle secondHandle = secondjob.Schedule(handle);
        // other tasks to run in Main Thread in parallel

        //handle.Complete();
        secondHandle.Complete();


        float resultingValue = resultArray[0];
        Debug.Log("result = " + resultingValue);
        Debug.Log("myJob.a = " + myJob.a);

        resultArray.Dispose();
    }

    private struct SimpleJob : IJob
    {

        public float a;

        public NativeArray<float> result;
        public void Execute()
        {
            result[0] = a;
            a = 23f;
        }
    }

    private struct AnotherJob: IJob
    {
        public NativeArray<float> result;

        public void Execute()
        {
            result[0] = result[0] + 1;
        }
    }
}
