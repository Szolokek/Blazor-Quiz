﻿using Kviz.Data;
using Kviz.Migrations.Tables;

namespace Kviz.Services
{
  public class InitializationService : IHostedService
  {
    private readonly IServiceProvider _serviceProvider;

    public InitializationService(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      using (var scope = _serviceProvider.CreateScope())
      {
        var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
        var quizService = scope.ServiceProvider.GetRequiredService<QuizService>();

        var sessions = await dataService.GetSessions();
        foreach (SessionTable session in sessions)
        {
          quizService.AddNewSession(session.Id);
        }
      }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }


}
