using Resit_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Resit_Project.Models
{
    public class Work
    {
        public int WorkId { get; set; }
        public int StaffId { get; set; }
        public int CateId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Category Category { get; set; }

        /*public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var stage in PriceLists)
                {
                    totalPrice += stage.Price * stage.Quantity;
                }
                return totalPrice;
            }
        }

        public void AddStage(PriceList priceList)
        {
            PriceLists.Add(priceList);
        }

        public void RemoveStage(PriceList priceList)
        {
            PriceLists.Remove(priceList);
        }*/


        /*[NotMapped]
        public List<PriceList> priceLists
        {
            get
            {
                if (Categories == null)
                {
                    return null;
                }
                return Categories.PricelistId.ToList();
            }
        }*/
    }
}

/*public decimal CalculatePrice()
{
    if (Category == null)
    {
        return 0;
    }
    decimal total = 0;
    foreach (var stage in Category.Stages)
    {
        var cateItem = stage.CateItems.FirstOrDefault(ci => ci.CateItemId == CateItemId);
        if (cateItem != null)
        {
            total += cateItem.Price * Quantity;
        }
    }
    return total;
}*/