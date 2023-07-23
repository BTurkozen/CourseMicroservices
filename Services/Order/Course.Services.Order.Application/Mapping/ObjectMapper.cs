using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Mapping
{
    // Sadece ihtiyaç olduğunda yükleme işlemi gerçekleştirilecek.
    public static class ObjectMapper
    {
        // Lazy İstendiğinde kullanılmasını sağlayan bir Class
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });

            return config.CreateMapper();
        });

        // Mapper çağrılana kadar Yukarıdaki lazy çalışmayacak. Taki nezamn Mapper çağrılırsa ozaman çalışmaya başlayacak.
        public static IMapper Mapper => lazy.Value;
    }
}
