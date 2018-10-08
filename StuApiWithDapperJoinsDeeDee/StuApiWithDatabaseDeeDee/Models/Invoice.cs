using System;
using System.Collections.Generic;
namespace StuApiWithDatabaseDeeDee.Models
{
    public class Invoice
    {
        public int INV_NUMBER { get; set; }
        public DateTime INV_DATE { get; set; }
        public List<LineItem> InvoiceLineItems { get; set; }
    }
}
