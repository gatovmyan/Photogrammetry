#include "Stepper_28BYJ.h"

// установить количество шагов 4078 для мотора 
#define STEPS 4078
// задать управляющие пины (Pin) на плате контроллера Ардуино
Stepper_28BYJ stepper(STEPS, 8, 9, 10, 11);
void setup()
{
  // установить скорость вращения двигателя 13 об/мин
  // как максимальное значение
  //stepper.setSpeed(13);

  // Инициализация последовательного порта с указанием скорости обмена данными ( по умолчанию лучше использовать 9600 бод)
  Serial.begin(9600);
  // Устанавливаем таймаут (значение по умолчанию слишком велико)
  Serial.setTimeout(100);
  
  stepper.setSpeed(5);
   
}
void loop()
{
  /*
  stepper.step(STEPS); // Задать вращение 4000 шагов по часовой стрелке
  delay(1000);
    stepper.step(STEPS); // Задать вращение 4000 шагов по часовой стрелке
  delay(1000);
    stepper.step(STEPS); // Задать вращение 4000 шагов по часовой стрелке
  delay(1000);
    stepper.step(STEPS); // Задать вращение 4000 шагов по часовой стрелке
  delay(1000);
    stepper.step(STEPS); // Задать вращение 4000 шагов по часовой стрелке
  delay(1000);
  */

    // Если поступили данные с ПК
  if (Serial.available() > 0) {
    // Считываем полученные данные
    String command = Serial.readString();
    Serial.println(command);

    stepper.step(5*STEPS/command.toInt());
    
    /*
    // Формируем ответ
    String response = "Command " + command + " is accepted!";
    // Отправляем ответ ПК
    char buf[response.length() + 1];
    response.toCharArray(buf, response.length() + 1);
    Serial.write(buf);
    */
  }
  
//  stepper.step(-4000);// Задать вращение 4000 шагов против часовой стрелки
// если одну из строк задания вращения исключить
// мотор станет вращаться без остановки
}
