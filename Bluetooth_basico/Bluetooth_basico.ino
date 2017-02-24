#include <SoftwareSerial.h>

char valorRecibido;
SoftwareSerial blue(0,1);
int relevador = 13;

void setup()
{
  pinMode(relevador,OUTPUT);
  blue.begin(9600);
  blue.println("Conectado");
}

void loop() 
{
  if(blue.available())
  {
    valorRecibido=blue.read();
    switch(valorRecibido)
    {
      case '0':
        digitalWrite(relevador,LOW);
        blue.println("Luz apagada");
        break;
      case '1':
        digitalWrite(relevador,HIGH);
        blue.println("Luz encendida");
        break;
      default:
        blue.print(rec);
        blue.println(" no es una orden valida. Introduzca 0 o 1");
    }
  }
  delay(500);
}
