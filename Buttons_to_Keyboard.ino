#include "Keyboard.h" 

const int buttonPin[] = {2, 3, 4, 5}; // Array of button pin numbers
int pinCount = 4;
int buttonState[] = {0, 0, 0, 0}; // Array to store current button states, used in debouncer
int prevButtonState[] = {HIGH, HIGH, HIGH, HIGH}; // Array to store previous button states, used in debouncer

long lastDebounceTime[] = {0, 0, 0, 0}; // Array to store the last debounce time for each button
long debounceDelay = 50; // Delay time for button debouncing

void setup() {
  for (int thisPin = pinCount - 1; thisPin >= 0; thisPin--) {
    pinMode(buttonPin[thisPin], INPUT); // Set button pins as input
    digitalWrite(buttonPin[thisPin], HIGH); // Enable internal pull-up resistors for the buttons
  }
  Keyboard.begin(); // Initialize the Keyboard library
}

// Output actions function - performs actions based on the current button
int outputAction(int currentButton) {
    if (currentButton == 0) {
      Keyboard.press(KEY_RIGHT_ARROW); // Press the corresponding arrow key
      delay(100); // Delay for 100 milliseconds in order to give the previous command time to register on keyboard.
      Keyboard.releaseAll(); // Release all keys
    }
    if (currentButton + 1 == 2) {
      Keyboard.press(KEY_DOWN_ARROW); 
      delay(100); 
      Keyboard.releaseAll(); 
    }
    if (currentButton + 1 == 3) {
      Keyboard.press(KEY_UP_ARROW);
      delay(100); 
      Keyboard.releaseAll(); 
    }
    if (currentButton + 1 == 4) {
      Keyboard.press(KEY_LEFT_ARROW); 
      delay(100);
      Keyboard.releaseAll();
    }
}

void loop() {
  // Button debouncer for buttons
  for (int thisPin = pinCount - 1; thisPin >= 0; thisPin--) {
    buttonState[thisPin] = digitalRead(buttonPin[thisPin]); // Read the current state of the button

    // Check if the button state has changed from the previous state and is now HIGH (button released)
    if ((buttonState[thisPin] != prevButtonState[thisPin]) && (buttonState[thisPin] == HIGH)) {
      // Check if enough time has passed since the last debounce for the button
      if ((millis() - lastDebounceTime[thisPin]) > debounceDelay) {
        outputAction(thisPin); // Perform an output action based on the button
        lastDebounceTime[thisPin] = millis(); // Update the last debounce time for the button
      }
    }

    prevButtonState[thisPin] = buttonState[thisPin]; // Update the previous button state
  }
}
