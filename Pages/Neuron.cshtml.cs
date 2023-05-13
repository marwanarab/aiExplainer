using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class NeuronModel : PageModel
{
    private readonly NeuralNetwork _nn;

    public int NeuronId { get; set; }
    [BindProperty]
    public int NeuronValue { get; set; }

    [BindProperty]
    public Dictionary<int, bool> Connections { get; set; } = new Dictionary<int, bool>();

    public Dictionary<int, int> NeuronValues { get => _nn.NeuronValues; }

    public int NeuronResult
    {
        get
        {
            int result = 0;
            foreach (var item in Connections)
            {
                if (item.Value)
                {
                    result += NeuronValues[item.Key];
                }
            }
            return result;
        }
    }

    public NeuronModel(NeuralNetwork globalState)
    {
        this._nn = globalState;
    }

    public IActionResult OnGet(int id)
    {
        NeuronId = id;
        NeuronValue = _nn.NeuronValues[id];

        // Load the existing connections into the Connections
        for (int i = 1; i <= 9; i++)
        {
            Connections.Add(i, _nn.Connections[NeuronId - 1, i-1]);
        }

        return Page();
    }

    public IActionResult OnPost(int id)
    {
        NeuronId = id;
        _nn.NeuronValues[id] = NeuronValue;

        // Save the connections.
        foreach (var item in Connections)
        {
            _nn.Connections[NeuronId - 1, item.Key-1] = item.Value;
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        return Page();
    }
}