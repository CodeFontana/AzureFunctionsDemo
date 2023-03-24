namespace DurableFunctionDemo.Models;
public class GreetingModel
{
    public GreetingModel(string name, string greeting)
    {
        Name = name;
        Greeting = greeting;
    }

    public string Name { get; set; }

    public string Greeting { get; set; }
}
