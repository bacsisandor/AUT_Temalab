button = None

"""Describe this function...
"""
def say_hello():
  global button
  print('Hello!')


button = Button(21)
while True:
  if button.is_pressed:
    say_hello()
