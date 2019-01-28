#include <ZumoShield.h>
#include <NewPing.h>
#include <Wire.h>

#define LED 13
#define SPEED 100
#define EDGELIMIT 500 
#define WALLLIMIT 1500 
#define TRIGGERPIN  2  // Sensor trigger pin.
#define ECHOPIN     6  // Sensor echo pin.
#define DIST 200 

ZumoBuzzer buzzer;
ZumoMotors motors;
NewPing sonar(TRIGGERPIN, ECHOPIN, DIST); // NewPing setup of pins and maximum distance.

unsigned int sensors[6];
ZumoReflectanceSensorArray reflectSensors(QTR_NO_EMITTER_PIN);
int msg;
void setup() 
{
  msg = 0;
  Serial.begin(9600);
  while(!Serial);
}
  
void loop() 
{
  msg = 0;
  if(Serial.available() > 0)    
    {
       msg = Serial.read();
       handleMsg(msg);
    }
    
}

void handleMsg(int _msg)
{
  if(msg == 'f')
  {
    Serial.println("Move Forward");  
    motors.setSpeeds(SPEED, SPEED);
  }
  else if(msg == 'l')
  {
    Serial.println("Turn Left");  
    motors.setSpeeds(-SPEED, SPEED);
  }
  else if(msg == 'r')
  {
    Serial.println("Turn Right");  
    motors.setSpeeds(SPEED, -SPEED);
  }
  else if(msg == 'b')
  {
    Serial.println("Move Back");  
    motors.setSpeeds(-SPEED, -SPEED);
  }
  else if(msg == 'a')
  {
    autoForward();
    Serial.println("Automated");
  }
  else if(msg == 'i')
  {
    Serial.println("Interrupt");
    motors.setSpeeds(0, 0);
  }    
  else if(msg == 'o')
  {
    Serial.println("Scan Left");
    searchRoom(-1); //Inverse values
  }  
  else if(msg == 'p')
  {
    searchRoom(1);
    Serial.println("Scan Right");
  }
}

void autoForward()
{
   bool interrupt = false;
  while (!interrupt)
  {
    if(msg == 'i')
      interrupt = true;
      
    reflectSensors.read(sensors);
    
    float avg = sensors[0] +
                sensors[1] +
                sensors[2] +
                sensors[3] +
                sensors[4] +
                sensors[5];
                
    avg = avg / 6;
    
    if (avg > EDGELIMIT)
      interrupt = true;
      
    else if (sensors[0] > EDGELIMIT)
    {
      // Left Sensors
      motors.setSpeeds(-SPEED, -SPEED);
      delay(100);
      motors.setSpeeds(SPEED, -SPEED);
      delay(250);
      motors.setSpeeds(SPEED, SPEED);
    }
    else if (sensors[5] > EDGELIMIT)
    {
      // Right Sensors
      motors.setSpeeds(-SPEED, -SPEED);
      delay(100);
      motors.setSpeeds(-SPEED, SPEED);
      delay(250);
      motors.setSpeeds(SPEED, SPEED);
    }
    else
    {
      //Straight Ahead
      motors.setSpeeds(SPEED, SPEED);
      msg = Serial.read();
    }

  }
  //Pause the Zumo
  motors.setSpeeds(0, 0);
  msg ='i';
}

//Modifier to adjust left / right rotations
void searchRoom(int modifier)
{
    bool objInRoom = false;
  
  motors.setSpeeds(SPEED, SPEED);
  delay(200);
  motors.setSpeeds(modifier * SPEED, modifier * -SPEED);
  delay(400);
 

  for(int i = 0; i < 30; i++)
  {
    delay(50);
    float range = sonar.ping_cm();
    if(range < 17.5 && range > 2)
    {
        objInRoom = true;
        buzzer.play("L16 cdegreg4");
        Serial.println('z');
    }

  }
  
  motors.setSpeeds(modifier * SPEED, modifier * -SPEED);
  for(int i = 0; i < 30; i++)
  {
    delay(50);
    float range = sonar.ping_cm();
    if(range < 15 && range > 2)
      objInRoom = true;
  }
  motors.setSpeeds(0, 0);

  
}
