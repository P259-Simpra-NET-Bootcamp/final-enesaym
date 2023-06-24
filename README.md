SimShop Api 

Postman documantation link : https://documenter.getpostman.com/view/27227009/2s93z6cP6Q

Sql baglantısı için appsettings içerisindeki   "MsSqlConnection": "Server=" localinizdeki server ayarlanmalıdır.

Code first veritabanlarının olusturulması için powershell arayüzüne asagıdaki kod girilmelidir :

dotnet ef database update --project "./SimShop” --startup-project "./SimShop"

Adım 1:
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/02eb9020-8166-45f1-9ad6-381e75988d58)
Sistem senaryosu admin yetkisine sahip kullanıcının admin,admin kullanıcı adı ve şifresini girerek login olması ile başlar. 
Get : Giriş yapmış kullanıcının bilgilerini getirir.
Post(SingIn): Kullanıcının giriş yaparak accessToken almasını sağlar.
Post(SıgnUp): Kullanıcının sisteme customer yetkisiyle kayıt olmasını sağlar.

Adım 2:
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/3d9a83e6-b098-47de-83c7-2c76828e4a25)
Admin yetkisi ile giriş yapıldıktan sonra ürün eklenebilmesi için categoryler eklenir. Categoryler id ye göre güncellenebilir veya silinebilir. 

Adım 3:
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/a0c64431-9e4a-49d9-97bf-9aee72282acd)
Categoryler eklendikten sonra ürünler eklenir. Her ürünün bir kategoriye ait olması gereklidir. 
Ürünler category id ye göre listelenebilir. Ürün id ye göre güncellenebilir veya silinebilir.

Adım 4:
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/1b1d992a-f4db-4337-8ccb-60302f2307a8)
Admin yetkisine sahip kullanıcılar sistemdeki kullanıcılara Post metodu ile kupon tanımlayabilir. Get metodu ile sistemdeki aktif kullanıcı kendisine tanımlanan kuponları görüntüleyebilir.

Adım 5:
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/cd33947a-ec2e-49f4-aa87-ff196d5c0fc6)
Bu adımdan sonra customer rölüne sahip kullanıcılar sepetlerine ürün ekleyip cıkarabilir ve sepet içerisindeki ürünleri görüntüleyebilir.

Adım 6:
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/4eec8032-a263-475f-b8cf-1bcc1ca841e9)
Kullanıcı sepetindeki ürünleri order post ile sipariş verebilir. Sipariş verildikten sonra kullanıcıya ait siparişler get apisi ile görüntülenebilir. Sipariş verildikten sonra kullanıcı sepeti temizlenir. Sipariş sırasında kupon kodu girilirse kupon kodu ve cüzdan bakiyesi kadar indirim yapılır. Kullanılan puanlara ve kupon kodlarına göre puan kazanımı cüzdan hesabına aktarilir.

Extra : 
![image](https://github.com/P259-Simpra-NET-Bootcamp/final-enesaym/assets/89969736/b6a0259a-bf12-4e68-b8c4-373f96b8fc8c)
Admin yetkisine sahip kullanıcı sisteme admin yetkisine sahip kullanıcı ekleyebilir veya tüm kullanıcılar üzerinde silme guncelleme ve getirme işlemlerini yapabilir.

