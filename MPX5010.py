from machine import Pin, ADC
from time import sleep

# Configuración del pin del sensor y del ADC (sensor conectado al pin 34 en este caso)
sensor_pin = ADC(Pin(34))
sensor_pin.atten(ADC.ATTN_11DB)  # Configurar para leer hasta 3.3V
sensor_pin.width(ADC.WIDTH_10BIT)  # Resolución de 10 bits (lecturas de 0 a 1023)

# Leer el valor de offset en condiciones de presión cero
sensor_offset = sensor_pin.read() * (3.3 / 1023.0)  # Calcular el voltaje de offset

def PRESION():
    """Función que lee y devuelve la presión en mbar del sensor MPX5010."""
    sensor_value = sensor_pin.read()  # Leer valor del sensor
    voltage = sensor_value * (3.3 / 1023.0)  # Convertir a voltaje

    # Ajustar el voltaje restando el offset
    adjusted_voltage = voltage - sensor_offset

    # Convertir el voltaje ajustado a presión en kPa usando la fórmula del datasheet
    pressure_kPa = (adjusted_voltage - 0.2) * (10.0 / (4.7 - 0.2))

    # Convertir la presión de kPa a mbar
    pressure_mbar = pressure_kPa * 10.0

    return pressure_mbar
