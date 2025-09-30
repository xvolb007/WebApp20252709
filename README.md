1) In CalculationService I didn’t had time to investigate whether it makes sense to use DecimalEx instead of simply casting 
  to double and relying on Math functions, or how much precision might be lost in that case. 
  Therefore, I decided to stick with DecimalEx. Since it does not provide a cube root method, 
  I used Pow instead. However, for some reason DecimalEx.Pow throws an error when 0 is passed 
  as the base, even though mathematically 0^(1/3) should evaluate to 0.
2) Also, if we pass 1 into the input multiple times, we will get 0 in the computed value because ln(1) = 0.  
  This value is then stored, and on the next call when we calculate ln(x) / storedValue, we end up dividing by zero.  
  Right now it throws an error because this input is unexpected, but alternatively we could just return a message to the user indicating that dividing by zero is not valid
