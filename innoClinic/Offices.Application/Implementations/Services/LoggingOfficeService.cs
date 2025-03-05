using Microsoft.Extensions.Logging;
using Offices.Application.Dtos;
using Offices.Application.Interfaces.Services;
using System.Diagnostics;


namespace Offices.Application.Implementations.Services {
    public sealed class LoggingOfficeService: IOfficeService {
        private readonly IOfficeService _officeService;
        private readonly ILogger<LoggingOfficeService> _logger;
        private const string ActivityCode = "100100";
        private List<string> _excludedStartWith = new() {
            "at System.",
            "at Microsoft.",
            "at MongoDB."
        };
        public LoggingOfficeService( IOfficeService officeService, ILogger<LoggingOfficeService> logger ) {
            this._officeService = officeService;
            this._logger = logger;
        }

        public async Task<string> CreateAsync( CreateOfficeDto officeDto ) {
            return await RunAndLogAsync( async () => await _officeService.CreateAsync( officeDto ), nameof( CreateAsync ) );
        }

        public async Task DeleteAsync( string id ) {
            await ExecuteAndLogAsync( _officeService.DeleteAsync(id), nameof(DeleteAsync));
        }

        public async Task<List<OfficeDto>> GetAllAsync() {
            return await RunAndLogAsync( async ()=>await _officeService.GetAllAsync() , nameof(GetAllAsync));
        }

        public async Task<OfficeDto> GetAsync( string id ) {
            return await RunAndLogAsync( async () => await _officeService.GetAsync(id), nameof( GetAsync ) );
        }
        public async Task<PagedOfficesDto> GetPageAsync( int skip, int take ) {
            return await RunAndLogAsync( async () => await _officeService.GetPageAsync(skip, take), nameof( GetPageAsync ) );
        }
        public async Task UpdateAsync( string id, UpdateOfficeDto officeDto ) {
            await ExecuteAndLogAsync(_officeService.UpdateAsync(id, officeDto), nameof(UpdateAsync));
        }

        public async Task SetPathToOffice( string id, string path ) {
            await ExecuteAndLogAsync( _officeService.SetPathToOffice( id, path ), nameof( SetPathToOffice ) );
        }

        private async Task ExecuteAndLogAsync( Task action, string methodName ) {
            long timestamp = Stopwatch.GetTimestamp();

            await action;

            LogElapsedTime( timestamp, methodName );
        }

        private void ExecuteAndLog( Action action, string methodName ) {
            long timestamp = Stopwatch.GetTimestamp();

            action();

            LogElapsedTime( timestamp, methodName );
        }

        private async Task<T> RunAndLogAsync<T>( Func<Task<T>> func, string methodName ) {
            long timestamp = Stopwatch.GetTimestamp();

            var result = await func();

            LogElapsedTime( timestamp, methodName );

            return result;
        }

        private void LogElapsedTime( long timestamp, string methodName ) {
            var elapsedTime = Stopwatch.GetElapsedTime(timestamp);

            var stackTrace = FilterStackTrace(Environment.StackTrace);


            var logMessage = string.Format( 
                format: "Code {4}\t Service\t{0}\tmethod\t{1}\tcompleted with\t{2}\ttime.\n\tStackTrace{3}",
                nameof( LoggingOfficeService ),
                methodName,
                elapsedTime.ToString(),
                stackTrace, 
                ActivityCode);

            _logger.LogInformation( logMessage );
        }

        private string FilterStackTrace( string stackTrace ) {
            var lines = stackTrace.Split( new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            var filteredLines = lines.Where( line => !_excludedStartWith.Any( exclusion=>line.Contains( exclusion ) ) ).ToList();
            return string.Join( Environment.NewLine, filteredLines );
        }
    }
}
