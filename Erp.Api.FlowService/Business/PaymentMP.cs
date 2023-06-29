using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Application.Dtos.Flow.Mp;
using Erp.Api.Domain.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Erp.Api.FlowService.Business
{
    public class PaymentMP
    {
        private readonly PaymentIntent _paymentIntent;
        private readonly CobPo _point;
        private readonly bool _produccion;

        public PaymentMP(PaymentIntent paymentIntent, CobPo point, bool produccion)
        {
            _paymentIntent = paymentIntent;
            _point = point;
            _produccion = produccion;
        }

        public async Task<PaymentIntentResponse> PagoMP()
        {
            PaymentIntentResponse? intento = await CreatePaymentIntent();
            if (intento == null)
            {
                return null!;
            }

            int intentos = 0;

            while (intentos <= 30)
            {
                StateIntentPay? estados = await StatePaymentIntent(intento.Id!);
                switch (estados.Status)
                {
                    case "OPEN" or "PROCESSING" or "ON_TERMINAL" or "PROCESSED":
                        intentos += 1;
                        Thread.Sleep(5000);
                        continue;
                    case "CANCELED" or "ERROR":
                        await CancelPaymentIntent(intento.Id!);
                        intento.Status = estados.Status;
                        return intento;
                    case "FINISHED":
                        intento.Status = estados.Status;
                        return intento;
                    default:
                        intento.Status = estados.Status;
                        return intento;
                }
            }
            return intento;
        }
        private async Task<PaymentIntentResponse> CreatePaymentIntent()
        {
            Events? events = await GetPaymentIntents();
            if (events.EventList != null)
            {
                foreach (Event? evento in events!.EventList!)
                {
                    if (evento.Status == "OPEN")
                    {
                        await CancelPaymentIntent(evento.Payment_intent_id!);
                    }
                }
            }
            using (HttpClient? httpClient = new())
            {
                using HttpRequestMessage? request = new(new HttpMethod("POST"), $"https://api.mercadopago.com/point/integration-api/devices/{_point.DeviceId}/payment-intents");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_point.Token}");
                if (!_produccion) request.Headers.TryAddWithoutValidation("x-test-scope", "sandbox"); //borrar en produccion
                request.Content = JsonContent.Create(_paymentIntent);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                using HttpResponseMessage? response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        PaymentIntentResponse? result = await response.Content.ReadFromJsonAsync<PaymentIntentResponse>();
                        return result!;

                    }
                    catch (NotSupportedException) // When content type is not valid
                    {
                        Console.WriteLine("The content type is not supported.");
                        return null;
                    }
                    catch (JsonException) // Invalid JSON
                    {
                        Console.WriteLine("Invalid JSON.");
                        return null;
                    }
                }
            }
            return null!;
        }
        private async Task<Events> GetPaymentIntents()
        {
            using HttpClient? httpClient = new();
            using HttpRequestMessage? request = new(new HttpMethod("GET"), $"https://api.mercadopago.com/point/integration-api/payment-intents/events?startDate={DateTime.Today:yyyy-MM-dd}&endDate={DateTime.Today:yyyy-MM-dd}");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_point.Token}");
            if (!_produccion) request.Headers.TryAddWithoutValidation("x-test-scope", "sandbox"); //borrar en produccion
            using HttpResponseMessage? response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    Events? events = await response.Content.ReadFromJsonAsync<Events>();
                    return events;

                }
                catch (NotSupportedException) // When content type is not valid
                {
                    Console.WriteLine("The content type is not supported.");
                    return null!;
                }
                catch (JsonException) // Invalid JSON
                {
                    Console.WriteLine("Invalid JSON.");
                    return null;
                }
            }
            return null;
        }
        private async Task<CancelIntentPay> CancelPaymentIntent(string paymentIntent)
        {
            using (HttpClient? httpClient = new())
            {
                using HttpRequestMessage? request = new(new HttpMethod("DELETE"), $"https://api.mercadopago.com/point/integration-api/devices/{_point.DeviceId}/payment-intents/{paymentIntent}");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_point.Token}");
                if (!_produccion) request.Headers.TryAddWithoutValidation("x-test-scope", "sandbox"); //borrar en produccion
                using HttpResponseMessage? response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        CancelIntentPay? result = await response.Content.ReadFromJsonAsync<CancelIntentPay>();
                        return result!;
                    }
                    catch (NotSupportedException) // When content type is not valid
                    {
                        Console.WriteLine("The content type is not supported.");
                    }
                    catch (JsonException) // Invalid JSON
                    {
                        Console.WriteLine("Invalid JSON.");
                    }
                }
            }
            return null!;
        }
        private async Task<StateIntentPay> StatePaymentIntent(string paymentIntent)
        {

            using (HttpClient? httpClient = new())
            {
                using HttpRequestMessage? request = new(new HttpMethod("GET"), $"https://api.mercadopago.com/point/integration-api/payment-intents/{paymentIntent}/events");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_point.Token}");
                if (!_produccion) request.Headers.TryAddWithoutValidation("x-test-scope", "sandbox"); //borrar en produccion
                using HttpResponseMessage? response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        StateIntentPay? result = await response.Content.ReadFromJsonAsync<StateIntentPay>();
                        return result!;

                    }
                    catch (NotSupportedException) // When content type is not valid
                    {
                        Console.WriteLine("The content type is not supported.");
                    }
                    catch (JsonException) // Invalid JSON
                    {
                        Console.WriteLine("Invalid JSON.");
                    }
                }
            }
            return null!;
        }
    }
}
