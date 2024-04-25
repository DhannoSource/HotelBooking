using Hotel.RabbitMQ;

namespace Transaction.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IRabbitMQConsumer mQConsumer { get; set; }

        public static IApplicationBuilder UseAzServiceBusConsumer(this IApplicationBuilder app)
        {
            mQConsumer = app.ApplicationServices.GetService<IRabbitMQConsumer>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            
            return app;
        }

        private static void OnStarted()
        {
            mQConsumer.Start();
        }

    }
    }
