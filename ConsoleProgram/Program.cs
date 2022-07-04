// See https://aka.ms/new-console-template for more information

var encoding = System.Text.Encoding.UTF8;
byte[] b = new byte[2];
for (byte i = 0; i < 255; i++) {
    b[0] = i;
    Console.WriteLine(i +  "\t" + encoding.GetString(b));    
}

