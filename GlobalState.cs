public class NeuralNetwork
{
    public Dictionary<int, int> NeuronValues { get; set; } = new Dictionary<int, int>();
    public bool[,] Connections { get; set; } = new bool[9, 9];

    public NeuralNetwork()
    {
        for (int i = 1; i <= 9; i++)
        {
            NeuronValues.Add(i, 0);
            Connections[i - 1, i - 1] = true;
        }
    }

    public int this[int k]
    {
        get
        {
            int result = 0;
            for (int i = 1; i <= 9; i++)
            {
                if (Connections[k - 1, i - 1])
                {
                    result += NeuronValues[i];
                }
            }
            return result;
        }
    }

    public int GlobalResult
    {
        get
        {
            int result = 0;
            for (int i = 1; i <= 9; i++)
            {
                result += this[i];
            }
            return result;
        }
    }

}