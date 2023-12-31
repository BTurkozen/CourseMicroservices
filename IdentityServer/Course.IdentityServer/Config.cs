﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
                new ApiResource("resource_photo_stock"){ Scopes = { "photo_stock_fullpermission" } },
                new ApiResource("resource_basket") { Scopes = {"basket_fullpermission"} },
                new ApiResource("resource_discount"){Scopes = {"discount_fullpermission"} },
                new ApiResource("resource_order"){Scopes = {"order_fullpermission"} },
                new ApiResource("resource_payment"){Scopes = {"payment_fullpermission"} },
                new ApiResource("resource_gateway"){Scopes = { "gateway_fullpermission" } },
                // Yetkilendirmeyi bu şekilde de çoklu ve bölünmüş olarak verilebilmektedir.
                //new ApiResource("resource_discount"){Scopes = {"discount_fullpermission, discount_read,discount_write"} },
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
                new ApiScope("catalog_fullpermission","Catalog Api için Tam Yetki"),
                new ApiScope("photo_stock_fullpermission","Photo Stock Api için Tam Yetki"),
                new ApiScope("basket_fullpermission", "Basket Api için Tam Yetki"),
                new ApiScope("discount_fullpermission", "Discount Api için Tam Yetki"),
                new ApiScope("order_fullpermission", "Order Api için Tam Yetki"),
                new ApiScope("payment_fullpermission", "Payment Api için Tam Yetki"),
                new ApiScope("gateway_fullpermission", "Gateway Api için Tam Yetki"),
                //new ApiScope("discount_read", "Discount Api için Tam Yetki"),
                //new ApiScope("discount_write", "Discount Api için Tam Yetki"),
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
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission", "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
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
                    AllowedScopes = {
                        "basket_fullpermission",
                        "order_fullpermission",
                        "gateway_fullpermission", 
                        IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,"roles"},
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
                },
                new Client
                {
                    ClientName = "Token Exchange Client",
                    ClientId = "TokenExchangeClient",
                    ClientSecrets = { new Secret ("secret".Sha256())},
                    AllowedGrantTypes = new[]{"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes = {
                        "discount_fullpermission",
                        "payment_fullpermission",
                        IdentityServerConstants.StandardScopes.OpenId,
                    }
                },
            };
    }
}