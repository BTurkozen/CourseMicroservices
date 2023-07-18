// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Course.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog"){Scopes = { "catalog_fullpermission" } },
                new ApiResource("photo_stock"){ Scopes = { "photo_stock_fullpermission" } },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            };

        // Üyelik ile ilgili işlemleri burada tanımlıyoruz.
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       // Email OLmalı
                       new IdentityResources.Email(),
                       // Bunun Payload içerisinde olmak zorunda. SubKeyWord'un dolu olması gereklidir.
                       new IdentityResources.OpenId(),
                       // Kullanıcı Profile bilgilerine erişim sağlayabilsin.
                       new IdentityResources.Profile(),
                       // Custom Olarak Resource ekleme işlemi yapıyoruz. Yukarıdakiler tanımlı olan Resourceler toplu şekilde ekleme işlemi sağlanabiliyor.
                       // JWT'nin payloadundaki kullanıcı bilgiler bizim için claim nesnesidir.
                       // Burada Claim eklerken hangi Claim ile mapleneceğine karar veriyoruz 
                       new IdentityResource(){Name = "roles", DisplayName = "Roles", Description = "User Roles", UserClaims = new []{"role"} },
                   };

        // Client içerisinde bunun ekleme işlemide gerçekleştiriliyor.

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog Api için Full Erişim"),
                new ApiScope("photo_stock_fullpermission","Photo Stock Api için Full Erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId = "WebMvcClient",
                    ClientSecrets = { new Secret ("secret".Sha256())},
                    // ClientCredential'da Refresh Token Yoktur.
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId = "WebMvcClientForUser",
                    // OfflineAccess kullanıyoruz fakat buradan, Kullanılabilmesi için izin verilmesi gerekmektedir.
                    AllowOfflineAccess = true,
                    ClientSecrets = { new Secret ("secret".Sha256())},
                    // Password' içerisinde bulunmaktadaır.
                    // ResourceOwnerPasswordAndClientCredentials
                    // Bunu tanımlamamamızın sebebi Password için refresh token kullanabilmemiz gerekmesidir. Tanımlanması durumunda kullanamayacağız.
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    // Burada hangi izinlere müsade edeceğimiz kısmını dolduruyoruz.
                    // IdentityServerConstants.StandardScopes.OfflineAccess Refresh token için eklenmektedir. Kullanıcı offline olsa dahi kullanıcı adına yeni bir token alabiliriz istek atabiliriz. Burada elimizde refresh token olmadığı zaman Kullanıcıdan Email ve Password almak zorunda olmamak için buna ihtiyaç duyuyoruz.
                    AllowedScopes = {IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess,"roles",IdentityServerConstants.LocalApi.ScopeName },
                     // Access Token süresini belirliyoruz. 1 saat olarak ayarlıyoruz.
                     AccessTokenLifetime = 1*60*60,
                     // Refresh Token süresini belirliyoruz.
                     // TokenExpiration.Sliding => Refresh Token istendikçe AccessToken süresini otomatik artırır.
                     // TokenExpiration.Absolute => Kesin Bir tarih verilmek istenirse kullanılır. 
                     RefreshTokenExpiration = TokenExpiration.Absolute,
                     // Absolute verdiğimiz için burada süresini belirtiyoruz.
                     AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddMonths(1) - DateTime.Now).TotalSeconds,
                     // RefreshToken Birkeremi kullanılsın yoksa Arka Arkaya kullanılabilsin mi?
                     // TokenUsage.ReUse => Tekrar Kullanılmasını sağlar..
                     // Token.OneTimeOnly => Bir kere kullanılmasını sağlar.
                     RefreshTokenUsage = TokenUsage.ReUse,
                }
            };
    }
}