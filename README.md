# Chili Web App
This is the USA Mesonet ASP.NET Core website.

## Database-First
```
cd Usa.chili.Web
dotnet ef dbcontext scaffold "server=localhost;port=3306;user=root;password=root;database=chili" Pomelo.EntityFrameworkCore.MySql -f --project ../Usa.chili.Domain
rm -f ../Usa.chili.Domain/chiliContext.cs
```

## Bundling/Minimization
```
cd Usa.chili.Web
dotnet bundle
```

## Library Manager Restore
```
cd Usa.chili.Web
libman restore
```

# Service location
`/etc/systemd/system/kestrel-chili.service`