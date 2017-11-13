FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app

ADD mc-root-cert.crt /usr/local/share/ca-certificates
ADD mc-idreg-cert.crt /usr/local/share/ca-certificates
RUN update-ca-certificates

EXPOSE 443
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Sma.Stm.ApiGateway.dll"]