using System;

namespace Covariance_ContravaricaneInDelegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Basic Concept
            // It’s important to note that covariance is a concept that can occur implicitly, without using any special keywords or syntax.

            // Let’s look at a simple example:

            //var personObject = new Person();    
            //var employeeObject = new Employee();
            //var managerObject = new Manager();  

            // as we can see, we can assign more the more derived Employee and Manager objects to the less derived Person object.This is the basic concept of 
            // inheritance.
            //personObject = employeeObject;
            //personObject = managerObject;
            #endregion


            // What is Contravariance in C#?
            // Contravariance in C# is the opposite of covariance. It allows us to reverse assignment compatibility preserved by covariance.

            personDelegate del = GreetPerson;


            var managerObject = new Manager();
            Person[] people = new Employee[5];
            people[0] = managerObject;
            // System.ArrayTypeMismatchException: 'Attempted to access an element as a type incompatible with the array.'
            Console.WriteLine(people[0].ToString()); // Compile error: Cannot implicitly convert type 'Person[]' to 'Employee[]'.


            Func<string, Manager> getManager = GetPersonManger;
            Func<string, Person> getPerson = GetPersonManger;
            // As we can see, covariance allows us to assign the GetEmployeeManager method to the getEmployee delegate,
            // even though they have different return types.It also allows us to assign the getManager delegate to the getEmployee delegate.
            // These operations are allowed since Manager derives from Employee.
            getPerson = getManager;


            Action<Person> evaluatePersonPerformance = EvaluatePerformance;
            Action<Manager> evaluateManagerPerformance = EvaluatePerformance;
            // As we can see, contravariance allows us to assign the EvaluatePerformance method to the evaluateManagerPerformance delegate,
            // even though they have different parameter types.It also allows us to assign the evaluateEmployeePerformance delegate to the evaluateManagerPerformance delegate.
            // These operations are allowed since Manager derives from Employee.
            evaluateManagerPerformance = evaluatePersonPerformance;
        }

        public class Person { }
        public class Employee : Person { }  
        public class Manager : Person { }   

        delegate void personDelegate(Employee employee);
        static void GreetPerson(Person person) 
        { 
            // Logic to greet person.
        }
        static Manager GetPersonManger(string personFullName) { return new Manager(); }
        static void EvaluatePerformance(Person person) { /* Logic to evaluate performance.*/ }
    }
}