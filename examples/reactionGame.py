import random

player_1_button = None
player_2_button = None
led1 = None
timeToSleep = None


player_1_button = LED(4)
player_2_button = Button(21)
led1 = LED(20)
timeToSleep = random.random() * random.randint(1, 100)
sleep(timeToSleep)
led1.on()
while True:
  if player_1_button.is_pressed:
    print('Player 1 wins!')
    break
  if player_2_button.is_pressed:
    print('Player 2 wins!')
    break
led1.off()
