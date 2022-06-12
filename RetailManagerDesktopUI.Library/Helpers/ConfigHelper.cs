﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // TODO: Move from config to the API
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            var isValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (!isValidTaxRate)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            return output;
        }
    }
}
