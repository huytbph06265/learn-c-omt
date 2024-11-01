using System;
using System.Collections.Generic;

public class Publisher
{
    public event EventHandler<List<int>>? DataReceived;

    public void PublishData(List<int> data)
    {
        Console.WriteLine("Publisher phát hành dữ liệu mới.");
        DataReceived?.Invoke(this, data);
    }
}

public class Subscriber
{
    public void Subscribe(Publisher publisher, Action<List<int>> callback)
    {
        publisher.DataReceived += (sender, data) =>
        {
            callback(data); // Trả về danh sách mà không tính tổng
        };
    }
}

class Program
{
    static void Main()
    {
        var publisher = new Publisher();
        var subscriber = new Subscriber();

        // Đăng ký subscriber với callback để xử lý dữ liệu
        subscriber.Subscribe(publisher, data =>
        {
            int totalSum = 0;
            foreach (var number in data)
            {
                totalSum += number;
            }
            Console.WriteLine($"Tổng tính toán được trả về tại Subscribe: {totalSum}");
        });

        // Publisher phát hành dữ liệu mới
        var data1= new List<int> { 10, 20, 30, 40, 50 };
        publisher.PublishData(data1);
    }
}
