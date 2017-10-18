player_1_button = None
player_2_button = None
led1 = None
timeToSleep = None
lightSensor = None
pwmLed = None


lightSensor = LightSensor(20)
pwmLed = PWMLED(16)
while True:
  if 0 <= (lightSensor.values) and 25 > (lightSensor.values):
    pwmLed.value = 0.2
  elif 25 <= (lightSensor.values) and 50 > (lightSensor.values):
    pwmLed.value = 0.4
  elif 50 <= (lightSensor.values) and 75 > (lightSensor.values):
    pwmLed.value = 0.5
  else:
    pwmLed.value = 1
