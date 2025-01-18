using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VerySimpleFileManager.Helpers;

namespace VerySimpleFileManager.Workers;

public class FileIndexerWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public FileIndexerWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var fileIndexerHelper = _serviceProvider.GetService<FileIndexerHelper>();
            var commandLineArgumentHelper = _serviceProvider.GetService<CommandLineArgumentHelper>();

            if (commandLineArgumentHelper.Arguments.Count == 1)
            {
                var drive = fileIndexerHelper.Drives.SingleOrDefault(d => d.Name == commandLineArgumentHelper.Arguments[0]);
                if (drive != null)
                {
                    await Task.Run(async () => await fileIndexerHelper.ProcessDrive(drive));
                }
            }

            foreach (var drive in fileIndexerHelper.Drives)
            {
                if (!drive.IsIndexed)
                {
                    await Task.Run(async () => await fileIndexerHelper.ProcessDrive(drive));
                }
            }

            await Task.Delay(10_000, stoppingToken);
        }
    }
}