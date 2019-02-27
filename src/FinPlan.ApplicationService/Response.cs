namespace FinPlan.ApplicationService
{
    public class Response<T>
    {
        public bool IsSuccessful { get; set; }
        public T Result { get; set; }
    }
}