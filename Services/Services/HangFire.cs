using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class HangFire
    {
        /*
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs)
        {
            app.UseHangfireDashboard();
            backgroundJobs.Enqueue(() => CheckPrecompras());
        }

        [AutomaticRetry(Attempts = 0)]
        public void CheckPrecompras()
        {
            // Obtén las precompras que tienen más de 5 horas
            var precompras = _context.Precompras.Where(p => DateTime.Now - p.CreationTime > TimeSpan.FromHours(5));

            foreach (var precompra in precompras)
            {
                // Envía un correo electrónico al usuario
                SendEmail(precompra.UserEmail);
            }
        }
        */

    }
}
