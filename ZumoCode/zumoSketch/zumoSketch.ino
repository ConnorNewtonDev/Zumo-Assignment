#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand serialCmd;

void setup() 
{
  Serial.begin(9600);
  while(!Serial);
}
  
void loop() 
{
  if(Serial.available() > 0)    
    {
      serialCmd.readSerial();
      serialHandler();
    }
    
}

void serialHandler()
{
  char *arg;
  arg = serialCmd.next();

  switch(arg)
  {
    case "LT":
    {
      leftHandler();
      break;
    }
    case "RT":
    {
      rightHandler();
      break;
    }
    case "MF":
    {
      forwardHandler();
      break;
    }
  }
  
}
void leftHandler()
{  
  Serial.println("Left Turn");    
}

void rightHandler()
{  
  Serial.println("Right Turn");
}

void forwardHandler()
{
  Serial.println("Move Forward");
  //Check sensor
  //Adjust if needed
}
