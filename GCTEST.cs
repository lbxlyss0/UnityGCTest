using BoingKit;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

public class GCTEST : MonoBehaviour
{
    //测试环境:Unity 2018.4.22f1
    //测试数量
    int count = 100;

    private void Start()
    {
        #region 协程
        Profiler.BeginSample("Coroutines - Start");
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(Coroutines());
        }
        Profiler.EndSample();//6.3KB 
        #endregion

        #region 一系列New
        Profiler.BeginSample("New - String+");
        //会额外产生一次装箱操作
        for (int i = 0; i < count; i++)
        {
            int a = 100;//string也一样
            string b = a + "";
        }
        Profiler.EndSample();//5.1KB 

        Profiler.BeginSample("New - StringBuilder - Append");
        //New StringBuilder 和 Append 都会产生堆内存
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            sb.Append(i);
        }
        Profiler.EndSample();//3.8KB 

        Profiler.BeginSample("New - StringBuilder - ToString");
        //ToString会产生堆内存
        sb.ToString();
        Profiler.EndSample();//0.4KB 

        Profiler.BeginSample("New - string.format");
        //内部使用的就是StringBuilder
        for (int i = 0; i < count; i++)
        {
            string.Format("%", "123");
        }
        Profiler.EndSample();//2.7KB 

        Profiler.BeginSample("New - string.Concat");
        for (int i = 0; i < count; i++)
        {
            string a = string.Concat("a", "b");
        }
        Profiler.EndSample();//2.9KB 

        Profiler.BeginSample("New - array");
        for (int i = 0; i < count; i++)
        {
            int[] a = new int[i];
        }
        Profiler.EndSample();//22.5KB 

        Profiler.BeginSample("New - Class");
        for (int i = 0; i < count; i++)
        {
            GcTestClass a = new GcTestClass(i, i);
        }
        Profiler.EndSample();//2.3KB
        #endregion

        #region foreach
        List<Vector3> foreach_array = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            foreach_array.Add(new Vector3(i, i, i));
        }
        Profiler.BeginSample("foreach");
        foreach (var item in foreach_array) { }
        Profiler.EndSample();//48B
        #endregion

        #region Get
        Profiler.BeginSample("Get - obj.name");
        for (int i = 0; i < count; i++)
        {
            string a = transform.name;//gameObject.name一样
        }
        Profiler.EndSample();//3.7KB

        Profiler.BeginSample("Get - obj.tag");
        for (int i = 0; i < count; i++)
        {
            string a = transform.tag;//gameObject.name一样
        }
        Profiler.EndSample();//4.1KB

       
        #endregion

        #region 容易误认为GC的情况
        Profiler.BeginSample("WrongThink - New - String+");
        for (int i = 0; i < count; i++)
        {
            string a = "a" + "b";
        }
        Profiler.EndSample();

        Profiler.BeginSample("WrongThink - New - Struct");
        for (int i = 0; i < count; i++)
        {
            Vector3 a = new Vector3(i, i, i);
            Quaternion b = new Quaternion(i, i, i, i);
            GcTestStruct c = new GcTestStruct(i, i);
        }
        Profiler.EndSample();

        Profiler.BeginSample("WrongThink - Get - GetComponent");
        for (int i = 0; i < count; i++)
        {
            Transform a = transform.GetComponent<Transform>();
        }
        Profiler.EndSample();
       
        
        #endregion
    }

    //结构体的内存在：栈中开辟（适合：生命周期短，实例小的情况）
    struct GcTestStruct
    {
        public int a;
        public int b;
        public GcTestStruct(int a, int b) {
            this.a = a;
            this.b = b;
        }
    }

    //类的内存在：堆中开辟（适合：生命周期长，实例不小的情况）
    class GcTestClass
    {
        public int a;
        public int b;
        public GcTestClass(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }

    WaitForSecondsRealtime wfr = new WaitForSecondsRealtime(1f);
    IEnumerator Coroutines()
    {
        yield return wfr;
    }
}
