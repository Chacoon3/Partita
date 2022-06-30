// See https://aka.ms/new-console-template for more information

using System.Text;

var encoding = Encoding.UTF8;
byte[] b = new byte[2];
for (byte i = 0; i < 254; i++) {
    b[0] = i;
    b[1] = (byte)(i + 1);
    Console.WriteLine(i + "\t" + "i + 1" + "\t" + encoding.GetString(b));

}