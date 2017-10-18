player_1_button = None
player_2_button = None
led1 = None
timeToSleep = None
lightSensor = None
pwmLed = None
robot = None
distanceSensor = None


led1 = LEDled1
robot = Robot(left=(12, 16), right=(20, 21))
lightSensor = LightSensor(25)
distanceSensor = DistanceSensor(18,23)
while True:
  if (lightSensor.values) >= 0.2 and (distanceSensor.distance) > 0.2:
    robot.forward()
    led1.blink()
  else:
    robot.stop()
    led1.off()
