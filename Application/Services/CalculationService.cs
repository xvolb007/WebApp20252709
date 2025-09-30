using DecimalMath;
using Domain.DTOs;
using Domain.Storage;

namespace Application.Services
{
    public class CalculationService
    {
        public CalculationResponse Calculate(int key, decimal input)
        {
            var existing = GlobalStorage.Get(key);

            decimal previousValue = existing?.Value ?? 0;
            decimal newValue;

            if (existing == null || (DateTime.UtcNow - existing.Value.LastUpdated).TotalSeconds > 15)
            {
                newValue = 2;
            }
            else
            {
                decimal divisor = existing.Value.Value;
                if (divisor == 0)
                    throw new DivideByZeroException("Divisor value cannot be zero.");
                var ln = DecimalEx.Log(input);
                decimal baseValue = ln / divisor;
                // I didn’t have time to investigate whether it makes sense to use DecimalEx instead of simply casting 
                // to double and relying on Math functions, or how much precision might be lost in that case. 
                // Therefore, I decided to stick with DecimalEx. Since it does not provide a cube root method, 
                // I used Pow instead. However, for some reason DecimalEx.Pow throws an error when 0 is passed 
                // as the base, even though mathematically 0^(1/3) should evaluate to 0.
                if (baseValue == 0)
                {
                    newValue = 0;
                }
                else
                {
                    newValue = DecimalEx.Pow(baseValue, 1m / 3m);
                }
                var ln2 = Math.Log((double)input);
                var newValue1 = (decimal)Math.Cbrt(ln2 / (double)existing.Value.Value);
            }

            GlobalStorage.Set(key, newValue);

            return new CalculationResponse
            {
                ComputedValue = newValue,
                InputValue = input,
                PreviousValue = previousValue
            };
        }
    }
}
