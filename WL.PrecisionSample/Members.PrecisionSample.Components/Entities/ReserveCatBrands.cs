using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ReserveCatBrands
    {
        public int code { get; set; }
        public string message { get; set; } = string.Empty;
        public List<CompetitorBrand> competitorBrands { get; set; } = new List<CompetitorBrand>();
        public List<MainBrand> mainBrands { get; set; } = new List<MainBrand>();
    }

    //public class BrandData
    //{
    //    public List<CompetitorBrand> competitorBrands { get; set; } = new List<CompetitorBrand>();
    //    public List<MainBrand> mainBrands { get; set; } = new List<MainBrand>();
    //}

    public class Brand
    {
        public int brandId { get; set; }
        public int categoryId { get; set; }
        public string logoUrl { get; set; } = string.Empty;
    }

    public class CompetitorBrand
    {
        public Brand brand { get; set; } = new Brand();
        public string brandLabel { get; set; } = string.Empty;
    }

    public class MainBrand
    {
        public Brand brand { get; set; } = new Brand();
        public string catLabel { get; set; } = string.Empty;
        public string brandLabel { get; set; } = string.Empty;
        public Wordings wordings { get; set; } = new Wordings();
    }

    public class Wordings
    {
        public string peopleText { get; set; } = string.Empty;
        public string priceText { get; set; } = string.Empty;
        public string promotionText { get; set; } = string.Empty;
        public string productText { get; set; } = string.Empty;
        public string placementText { get; set; } = string.Empty;
        public string usecon12Text { get; set; } = string.Empty;
        public string usecon1Text { get; set; } = string.Empty;
        public string usecon2Text { get; set; } = string.Empty;
        public string usecon34Text { get; set; } = string.Empty;
        public string usecon3Text { get; set; } = string.Empty;
        public string usecon4Text { get; set; } = string.Empty;
        public string usecon56Text { get; set; } = string.Empty;
        public string usecon5Text { get; set; } = string.Empty;
        public string usecon6Text { get; set; } = string.Empty;
        public string volumetricsQuestionText { get; set; } = string.Empty;
        public string volumetricsLeftUnits { get; set; } = string.Empty;
        public string volumetricsRightUnits { get; set; } = string.Empty;
        public string sowQuestionText { get; set; } = string.Empty;
        public string volumetricsMaxValue { get; set; } = string.Empty;
        public string volumetricsMinValue { get; set; } = string.Empty;
        public string volumetricsBottomValue { get; set; } = string.Empty;
        public string volumetricsTopValue { get; set; } = string.Empty;
        public string volumetricsUnitText { get; set; } = string.Empty;
    }
}
