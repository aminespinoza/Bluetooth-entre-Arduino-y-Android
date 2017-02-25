#include <SoftwareSerial.h>

char valorRecibido;
SoftwareSerial blue(0,1);
int relevador = 8;

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
        blue.println("Luz encendida");
        break;
      case '1':
        digitalWrite(relevador,HIGH);
        blue.println("Luz apagada");
        break;
      default:
        blue.print(valorRecibido);
        blue.println(" no es una orden valida. Introduzca 0 o 1");
    }
  }
  delay(500);
}
