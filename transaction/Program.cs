using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

class ProductQuantity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public ProductQuantity(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

}


class Program
{
    static void Main()
    {
        // skapa en connectionstring mot min databas:
        string constring = File.ReadLines("connectionstring.txt").First();

        // skapa ett IDBConnection-objekt (dbcon) av MySQL-typ:
        using IDbConnection dbCon = new MySqlConnection(constring);

        dbCon.Open();

        int customerId = 5; // den här har vi fått från någonstans, här bara hårdkodar vi dom
        List<ProductQuantity> products = [new ProductQuantity(7, 2), new ProductQuantity(11, 1), new ProductQuantity(14, 5)];

        using var transaction = dbCon.BeginTransaction();

        int orderId = dbCon.ExecuteScalar<int>("insert into COrder(ODateTime, CustomerId) values(now(), @CustomerId); select last_insert_id();",
                                    new {CustomerId = customerId},
                                    transaction
                                    );

        foreach(ProductQuantity product in products)
        {
            dbCon.Execute("insert into ProductToOrder(COrderId, ProductId, Quantity) values (@OrderId, @ProductId, @Quantity)",
                            new{OrderId = orderId, ProductId = product.ProductId, Quantity = product.Quantity},
                            transaction
                            );
        }

        transaction.Commit();
    }
}