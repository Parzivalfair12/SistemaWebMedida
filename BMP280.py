from machine import Pin, I2C
import time
import struct

class BMP280:
    def __init__(self, i2c, address=0x76):
        self.i2c = i2c
        self.address = address
        self._read_calibration_data()
        self._setup()

    def _setup(self):
        # Configura el sensor BMP280 para empezar a medir
        self.i2c.writeto_mem(self.address, 0xF4, b'\x27')  # Configura el control del registro
        self.i2c.writeto_mem(self.address, 0xF5, b'\xA0')  # Configura el control de medición

    def _read_calibration_data(self):
        # Lee los coeficientes de calibración desde el sensor
        calib = self.i2c.readfrom_mem(self.address, 0x88, 24)
        self.dig_T1, self.dig_T2, self.dig_T3 = struct.unpack('<Hhh', calib[0:6])
        self.dig_P1, self.dig_P2, self.dig_P3, self.dig_P4, self.dig_P5, self.dig_P6, self.dig_P7, self.dig_P8, self.dig_P9 = struct.unpack('<Hhhhhhhhh', calib[6:24])

    def _read_raw_data(self):
        # Lee los datos crudos de presión y temperatura del sensor
        data = self.i2c.readfrom_mem(self.address, 0xF7, 6)
        raw_pressure = (data[0] << 12) | (data[1] << 4) | (data[2] >> 4)
        raw_temperature = (data[3] << 12) | (data[4] << 4) | (data[5] >> 4)
        return raw_temperature, raw_pressure

    def _compensate_temperature(self, raw_temperature):
        # Implementa la fórmula de compensación para la temperatura
        var1 = ((((raw_temperature >> 3) - (self.dig_T1 << 1))) * (self.dig_T2)) >> 11
        var2 = (((((raw_temperature >> 4) - (self.dig_T1)) * ((raw_temperature >> 4) - (self.dig_T1))) >> 12) * (self.dig_T3)) >> 14
        t_fine = var1 + var2
        temperature = (t_fine * 5 + 128) >> 8
        return temperature / 100.0, t_fine

    def _compensate_pressure(self, raw_pressure, t_fine):
        # Implementa la fórmula de compensación para la presión
        var1 = (t_fine >> 1) - 64000
        var2 = (((var1 >> 2) * (var1 >> 2)) >> 11) * self.dig_P6
        var2 = var2 + ((var1 * self.dig_P5) << 1)
        var2 = (var2 >> 2) + (self.dig_P4 << 16)
        var1 = (((self.dig_P3 * (((var1 >> 2) * (var1 >> 2)) >> 13)) >> 3) + ((self.dig_P2 * var1) >> 1)) >> 18
        var1 = ((32768 + var1) * self.dig_P1) >> 15
        if var1 == 0:
            return 0  # Evita división por cero
        pressure = ((1048576 - raw_pressure) - (var2 >> 12)) * 3125
        if pressure < 0x80000000:
            pressure = (pressure << 1) // var1
        else:
            pressure = (pressure // var1) * 2
        var1 = (self.dig_P9 * (((pressure >> 3) * (pressure >> 3)) >> 13)) >> 12
        var2 = ((pressure >> 2) * self.dig_P8) >> 13
        pressure = pressure + ((var1 + var2 + self.dig_P7) >> 4)
        return pressure / 100.0

    def read_temperature_and_pressure(self):
        raw_temperature, raw_pressure = self._read_raw_data()
        temperature, t_fine = self._compensate_temperature(raw_temperature)
        pressure = self._compensate_pressure(raw_pressure, t_fine)
        return temperature, pressure

def ATM():
    i2c = I2C(0, scl=Pin(22), sda=Pin(21))
    bmp = BMP280(i2c)
    pressure_reference = 562.0
    temperature_reference = 22.5
    initial_temperature, initial_pressure = bmp.read_temperature_and_pressure()
    pressure_offset = pressure_reference - initial_pressure
    temperature_offset = temperature_reference - initial_temperature
    
    while True:
        temperature_measured, pressure_measured = bmp.read_temperature_and_pressure()
        pressure_calibrated = pressure_measured + pressure_offset
        temperature_calibrated = temperature_measured + temperature_offset
        print("Temperature = {:.2f} ºC".format(temperature_calibrated))
        print("Pressure = {:.2f} mbar".format(pressure_calibrated))
        time.sleep(5)
        return temperature_calibrated, pressure_calibrated