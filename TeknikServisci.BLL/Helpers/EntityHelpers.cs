using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.BLL.Services.Senders
{
    public class EntityHelpers
    {
        public static string ValidationMessage(DbEntityValidationException ex)
        {
            var result = ex.EntityValidationErrors.Aggregate(string.Empty, (accumulator2, validationErrors) => validationErrors.ValidationErrors.Aggregate(accumulator2, (accumulator, validationError) => accumulator += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}\n"));
            return result + "\n";
        }
    }
}
