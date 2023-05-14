using aiExplainer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ResultsModel : PageModel
{
    private ImageService _imageService;
    public readonly NeuralNetwork _nn;
    private readonly IWebHostEnvironment _appEnvironment;

    string imageName = "dam-images-decor-libraries-library-06-sally-sirkin-lewis.webp";
    public string imagesFolder = "images/";
    public string newImageName = "newRender.webp";

    public string newImagePath { get { return Path.Combine(_appEnvironment.WebRootPath, imagesFolder, newImageName); } }

    public int numberOfSquares = 30;
    public int numberOfRearrangements = 30 * 30;

    public ResultsModel(IWebHostEnvironment appEnvironment, NeuralNetwork globalState)
    {
        this._appEnvironment = appEnvironment;
        this._nn = globalState;
        _imageService = new ImageService();
    }

    public IActionResult OnGet()
    {
        // ViewData["Title"] = "AI Explainer";
        NewMethod();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        NewMethod();
        return Page();
    }

    private void NewMethod()
    {
        var image = _imageService.LoadImage(Path.Combine(_appEnvironment.WebRootPath, imagesFolder, imageName));
        var squares = _imageService.SplitImage(image, numberOfSquares);
        var rearrangedSquares = _imageService.RearrangeSquares(squares, numberOfRearrangements - _nn.GlobalResult);
        var newImage = _imageService.MergeSquares(rearrangedSquares);

        _imageService.SaveImage(newImage, newImagePath);
    }
}
