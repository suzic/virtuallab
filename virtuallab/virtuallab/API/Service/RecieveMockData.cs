using Pocoor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using virtuallab.API.Service.po;

namespace virtuallab.API.Service
{
    public class RecieveMockData 
    {
        static List<String> data = new List<string>();
        static List<String> data2 = new List<string>();
        static int index = 0;
        static int index2 = 0;
        static RecieveMockData()
        {

            data.Add("正在 Ping www.a.shifen.com [61.135.169.121] 具有 32 字节的数据:");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"11111100,00000000,00000000,00000000\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"01100000,00000000,00000000,00000000\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"11011010,00000000,00000000,00000000\"}]");
            data.Add("来自 61.135.169.121 的回复: 字节=32 时间=19ms TTL=50");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"11110010,00000000,00000000,00000000\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"01100110,00000000,00000000,00000000\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"10110110,00000000,00000000,00000000\"}]");
            data.Add("<Console>[{\"wait\": 1000,\"value\":\"数据包: 已发送 = 4，已接收 = 4，丢失 = 0 (0% 丢失)\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"10111110,00000000,00000000,00000000\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"11100000,00000000,00000000,00000000\"}]");
            data.Add("<Console>[{\"wait\": 1000,\"value\":\"最短 = 19ms，最长 = 22ms，平均 = 20ms\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"11111110,00000000,00000000,00000000\"}]");
            data.Add("<Effect>[{\"wait\": 1000,\"value\":\"11110110,00000000,00000000,00000000\"}]");
            data.Add("程序运行结束");



            data2.Add("00000000,11111100,00000000,00000000,00000000");
            data2.Add("01000000,01100000,00000000,00000000,00000000");
            data2.Add("00100000,11011010,00000000,00000000,00000000");
            data2.Add("00010000,11110010,00000000,00000000,00000000");
            data2.Add("00001000,01100110,00000000,00000000,00000000");
            data2.Add("00000100,10110110,00000000,00000000,00000000");
            data2.Add("00000010,10111110,00000000,00000000,00000000");
            data2.Add("00000001,11100000,00000000,00000000,00000000");
            data2.Add("10000000,11111110,00000000,00000000,00000000");
            data2.Add("11000011,11110110,00000000,00000000,00000000");
        }

        public static ConsoleReceiveRes GetRecieve()
        {
            ConsoleReceiveRes res = new ConsoleReceiveRes();
            res.fail = 0;
            res.output = data[index];
            res.finish = index == data.Count - 1 ? 1 : 0;

            index++;
            if (index > data.Count - 1)
                index = 0;

            return res;
        }

        public static RunResultTickRes GetRecieve2()
        {
            RunResultTickRes res = new RunResultTickRes();
            res.fail = 0;
            res.effect = new List<string>();
            res.effect.AddRange(data2[index2].Split(','));

            index2++;
            if (index2 > data2.Count - 1)
                index2 = 0;

            return res;
        }
    }
}