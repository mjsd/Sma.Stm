FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .

ADD mc-root-cert.crt /usr/local/share/ca-certificates/
ADD mc-idreg-cert.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "Sma.Stm.Ssc.Services.Private.dll"]
