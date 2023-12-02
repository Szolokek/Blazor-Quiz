namespace Kviz.Services
{
    public class QuizServiceFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public QuizServiceFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public QuizService Create(IServiceProvider serviceProvider)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                return new QuizService();
            }
        }
    }
}
