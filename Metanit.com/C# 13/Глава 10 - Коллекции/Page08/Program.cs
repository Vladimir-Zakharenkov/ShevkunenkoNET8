#region Example 01

//Numbers numbers = new Numbers();

//foreach (int n in numbers)
//{
//    Console.WriteLine(n);
//}

//class Numbers
//{
//    public IEnumerator<int> GetEnumerator()
//    {
//        for (int i = 0; i < 6; i++)
//        {
//            yield return i * i;
//        }
//    }
//}

#endregion

#region Example 02

//foreach (var n in 5) Console.WriteLine(n);

//foreach (var n in -5) Console.WriteLine(n);

//static class Int32Extension
//{
//    public static IEnumerator<int> GetEnumerator(this int number)
//    {
//        int k = (number > 0) ? number : 0;

//        for (int i = number - k; i <= k; i++) yield return i;
//    }
//}

#endregion

#region Example 03

//var people = new Person[]
//{
//    new Person("Tom"),
//    new Person("Bob"),
//    new Person("Sam")
//};

//var microsoft = new Company(people);

//foreach (Person employee in microsoft)
//{
//    Console.WriteLine(employee.Name);
//}

//class Person
//{
//    public string Name { get; }

//    public Person(string name) => Name = name;
//}
//class Company
//{
//    Person[] personnel;

//    public Company(Person[] personnel) => this.personnel = personnel;

//    public int Length => personnel.Length;

//    //public IEnumerator<Person> GetEnumerator()
//    //{
//    //    for (int i = 0; i < personnel.Length; i++)
//    //    {
//    //        yield return personnel[i];
//    //    }
//    //}

//    public IEnumerator<Person> GetEnumerator()
//    {
//        yield return personnel[0];
//        yield return personnel[1];
//        yield return personnel[2];
//    }
//}

#endregion

#region Example 04

var people = new Person[]
{
    new Person("Tom"),
    new Person("Bob"),
    new Person("Sam")
};

var microsoft = new Company(people);

foreach (Person employee in microsoft.GetPersonnel(5))
{
    Console.WriteLine(employee.Name);
}

class Person
{
    public string Name { get; }
    public Person(string name) => Name = name;
}

class Company
{
    Person[] personnel;

    public Company(Person[] personnel) => this.personnel = personnel;

    public int Length => personnel.Length;

    public IEnumerable<Person> GetPersonnel(int max)
    {
        for (int i = 0; i < max; i++)
        {
            if (i == personnel.Length)
            {
                yield break;
            }
            else
            {
                yield return personnel[i];
            }
        }
    }
}

#endregion