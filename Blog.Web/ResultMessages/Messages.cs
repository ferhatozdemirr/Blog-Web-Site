namespace Blog.Web.ResultMessages
{
    public class Messages
    {
        public static class Article
        {
            public static string Add(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarı ile Eklenmiştir";
            }
            public static string Update(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarı ile Güncellenmiştir";
            }
            public static string Delete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarı ile Silinmiştir";
            }
            public static string UndoDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarı ile geri alınmıştır";
            }
        }
        public static class Category
        {
            public static string Add(string categoryName)
            {
                return $"{categoryName} başlıklı kategori başarı ile Eklenmiştir";
            }
            public static string Update(string categoryName)
            {
                return $"{categoryName} başlıklı kategori başarı ile Güncellenmiştir";
            }
            public static string Delete(string categoryName)
            {
                return $"{categoryName} başlıklı kategori başarı ile Silinmiştir";
            }
            public static string UndoDelete(string categoryName)
            {
                return $"{categoryName} başlıklı makale başarı ile geri alınmıştır";
            }
        }

        public static class User
        {
            public static string Add(string userName)
            {
                return $"{userName} Mail adressli kullanıcı başarı ile Eklenmiştir";
            }
            public static string Update(string userName)
            {
                return $"{userName} Mail adressli kullanıcı başarı ile Güncellenmiştir";
            }
            public static string Delete(string userName)
            {
                return $"{userName} Mail adressli kullanıcı başarı ile Silinmiştir";
            }
        }
    }
}
