using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetVariance
{
    class Program
    {
        static void Main(string[] args)
        {
            IReader<A> readerA = new Reader<A>();
            IReader<B> readerB = new Reader<B>();
            IReader<C> readerC = new Reader<C>();
            IWriter<A> writerA = new Writer<A>();
            IWriter<B> writerB = new Writer<B>();
            IWriter<C> writerC = new Writer<C>();
            IReaderWriter<A> readerWriterA = new ReaderWriter<A>();
            IReaderWriter<B> readerWriterB = new ReaderWriter<B>();
            IReaderWriter<C> readerWriterC = new ReaderWriter<C>();

           
            #region Covariance
            // IReader<TEntity> is Covariant, this means that:
            // 1. All members either don't deal with TEntity or have it in the return type, not the input parameters
            // 2. In a call, IReader<TEntity> could be replaced by any IReader<TAnotherEntity> given that TAnotherEntity
            // is a child -directly or indirectly- of TEntity

            // TestReader(readerB) is ok because TestReader is already expecting IReader<B>
            TestReader(readerB);

            // TestReader(readerC) is ok because C is a child of B
            TestReader(readerC);

            // TestReader(readerA) is NOT ok because A is a not a child of B
            TestReader(readerA);
            #endregion

            #region Contravariance
            // IWriter<TEntity> is Contravariant, this means that:
            // 1. All members either don't deal with TEntity or have it in the input parameters, not in the return type
            // 2. In a call, IWriter<TEntity> could be replaced by any IWriter<TAnotherEntity> given that TAnotherEntity
            // is a parent -directly or indirectly- of TEntity

            // TestWriter(writerB) is ok because TestWriter is already expecting IWriter<B>
            TestWriter(writerB);

            // TestWriter(writerA) is ok because A is a parent of B
            TestWriter(writerA);

            // TestWriter(writerC) is NOT ok because C is a not a parent of B
            TestWriter(writerC);
            #endregion

            #region Invariance
            // IReaderWriter<TEntity> is Invariant, this means that:
            // 1. Some members have TEntity in the input parameters and others have TEntity in the return type
            // 2. In a call, IReaderWriter<TEntity> could not be replaced by any IReaderWriter<TAnotherEntity>

            // IReaderWriter(readerWriterB) is ok because TestReaderWriter is already expecting IReaderWriter<B>
            TestReaderWriter(readerWriterB);

            // IReaderWriter(readerWriterA) is NOT ok because IReaderWriter<B> can not be replaced by IReaderWriter<A>
            TestReaderWriter(readerWriterA);

            // IReaderWriter(readerWriterC) is NOT ok because IReaderWriter<B> can not be replaced by IReaderWriter<C>
            TestReaderWriter(readerWriterC);
            #endregion
        }

        public static void TestReader(IReader<B> param)
        {
            var b = param.Read();
            b.F1();
            b.F2();

            // What if the compiler allows calling TestReader with a param of type IReader<A>, This means that:
            // param.Read() would return an instance of class A, not B
            //		=> So, the var b would actually be of type A, not B
            //		=> This would lead to the b.F2() line in the code above to fail as the var b doesn't have F2()

            // What if the compiler allows calling TestReader with a param of type IReader<C>, This means that:
            // param.Read() would return an instance of class C, not B
            //		=> So, the var b would actually be of type C, not B
            //		=> This would lead to the b.F2() line in the code above to work fine as the var b would have F2()
        }

        public static void TestWriter(IWriter<B> param)
        {
            var b = new B();
            param.Write(b);

            // What if the compiler allows calling TestWriter with a param of type IWriter<A>, This means that:
            // param.Write() line in the code above would be expecting to receive a parameter of type A, not B
            //		=> So, calling param.Write() while passing in a parameter of type A or B would both work

            // What if the compiler allows calling TestWriter with a param of type IWriter<C>, This means that:
            // param.Write() line in the code above would be expecting to receive a parameter of type C, not B
            //		=> So, calling param.Write() while passing in a parameter of type B would not work
        }

        public static void TestReaderWriter(IReaderWriter<B> param)
        {
            var b = param.Read();
            b.F1();
            b.F2();

            param.Write(b);

            // What if the compiler allows calling TestReaderWriter with a param of type IReaderWriter<A>, This means that:
            // 1. param.Read() would return an instance of class A, not B
            //		=> So, the var b would actually be of type A, not B
            //		=> This would lead to the b.F2() line in the code above to fail as the var b doesn't have F2()
            // 2. param.Write() line in the code above would be expecting to receive a parameter of type A, not B
            //		=> So, calling param.Write() while passing in a parameter of type A or B would both work

            // What if the compiler allows calling TestReaderWriter with a param of type IReaderWriter<C>, This means that:
            // 1. param.Read() would return an instance of class C, not B
            //		=> So, the var b would actually be of type C, not B
            //		=> This would lead to the b.F2() line in the code above to work fine as the var b would have F2()
            // 2. param.Write() line in the code above would be expecting to receive a parameter of type C, not B
            //		=> So, calling param.Write() while passing in a parameter of type B would not work
        }
    }

    #region Hierarchy Classes
    public class A
    {
        public void F1()
        {
        }
    }

    public class B : A
    {
        public void F2()
        {
        }
    }

    public class C : B
    {
        public void F3()
        {
        }
    }
    #endregion

    #region Covariant IReader
    // IReader<TEntity> is Covariant as all members either don't deal with TEntity or have it in the return type
    // not the input parameters
    public interface IReader<out TEntity>
    {
        TEntity Read();
    }

    public class Reader<TEntity> : IReader<TEntity> where TEntity : new()
    {
        public TEntity Read()
        {
            return new TEntity();
        }
    }
    #endregion

    #region Contravariant IWriter
    // IWriter<TEntity> is Contravariant as all members either don't deal with TEntity or have it in the input parameters
    // not the return type
    public interface IWriter<in TEntity>
    {
        void Write(TEntity entity);
    }

    public class Writer<TEntity> : IWriter<TEntity> where TEntity : new()
    {
        public void Write(TEntity entity)
        {
        }
    }
    #endregion

    #region Invariant IReaderWriter
    // IReaderWriter<TEntity> is Invariant as some members have TEntity in the input parameters
    // and others have TEntity in the return type
    public interface IReaderWriter<TEntity>
    {
        TEntity Read();
        void Write(TEntity entity);
    }

    public class ReaderWriter<TEntity> : IReaderWriter<TEntity> where TEntity : new()
    {
        public TEntity Read()
        {
            return new TEntity();
        }

        public void Write(TEntity entity)
        {
        }
    }
    #endregion
}