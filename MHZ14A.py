from machine import Pin, ADC, UART
from time import sleep

# Configuración de UART para la comunicación serial con el sensor
uart2 = UART(2, baudrate=9600, tx=17, rx=16)

# Comandos de calibración
cmd_zero_calibration = b'\xFF\x01\x87\x00\x00\x00\x00\x00\x78'
cmd_span_calibration = b'\xFF\x01\x88\x07\xD0\x00\x00\x00\xA0'

# Configuración del ADC para la lectura analógica del sensor
analog_pin = ADC(Pin(34))
analog_pin.width(ADC.WIDTH_12BIT)  # Resolución de 12 bits (0-4095)
analog_pin.atten(ADC.ATTN_11DB)    # Atenuación para medir hasta 3.3V
voltageStep = 3.3 / 4095.0  # Resolución del ADC

# Funciones de calibración
def calibrar_zero_point():
    print("Calibrando punto cero...")
    uart2.write(cmd_zero_calibration)  # Enviar comando de calibración de punto cero
    sleep(600)  # Esperar 10 minutos para estabilización
    print("Calibración de punto cero finalizada.")

def calibrar_span_point():
    print("Calibrando span...")
    uart2.write(cmd_span_calibration)  # Enviar comando de calibración de span
    sleep(600)  # Esperar 10 minutos para estabilización
    print("Calibración de span finalizada.")

# Lectura de la concentración de CO2 usando la salida analógica
def leer_concentracion_analogica():
    sensor_value = analog_pin.read()  # Leer el valor ADC
    voltage = sensor_value * voltageStep  # Convertir a voltaje
    co2_ppm = (voltage / 2.5) * 5000.0  # Calcular concentración de CO2 en ppm
    co2_percentage = co2_ppm / 10000.0  # Convertir ppm a porcentaje

    print("Voltaje: {:.2f}V".format(voltage))
    print("Concentración de CO2: {:.2f} ppm ({:.4f}%)".format(co2_ppm, co2_percentage))

    return co2_ppm, co2_percentage

# Bucle principal
def main():
    print("Iniciando el sistema...")
    
    # Si es necesario, puedes habilitar la calibración descomentando las líneas a continuación
    # calibrar_zero_point()
    # calibrar_span_point()

    while True:
        # Leer concentración de CO2
        co2_ppm, co2_percentage = leer_concentracion_analogica()
        
        # Esperar 10 segundos antes de la siguiente lectura
        sleep(10)

# Ejecutar el código
if __name__ == "__main__":
    main()
