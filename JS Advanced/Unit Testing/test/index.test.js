
const mathEnforcer = require('../src/index.js');
const { assert } = require('chai');

describe('mathEnforcer', function () {
    describe('addFive', function () {

        it('should return correct result with a non-number parameter', function () {
            assert(mathEnforcer.addFive('test') === undefined);
        });
        it('should return undefined with Array', function () {
            assert(mathEnforcer.addFive([]) === undefined);
        });
        it('should return undefined with an Object', function () {
            assert(mathEnforcer.addFive({}) === undefined);
        });
        it('should return undefined with an undefined', function () {
            assert(mathEnforcer.addFive(undefined) === undefined);
        });
        it('should return undefined with an null', function () {
            assert(mathEnforcer.addFive(null) === undefined);
        });


        it('Should return correct num for correct input', function () {
            assert(mathEnforcer.addFive(5) === 10);
        });
        it('Should return correct num for correct negative input', function () {
            assert(mathEnforcer.addFive(-5) === 0);
        });
        it('Should return correct num for decimal input', function () {
            assert(mathEnforcer.addFive(5.5) === 10.5);
        });
    });
    describe('subtractTen', function () {

        it('should return correct result with a non-number parameter', function () {
            assert(mathEnforcer.subtractTen('test') === undefined);
        });
        it('should return undefined with Array', function () {
            assert(mathEnforcer.subtractTen([]) === undefined);
        });
        it('should return undefined with an Object', function () {
            assert(mathEnforcer.subtractTen({}) === undefined);
        });
        it('should return undefined with an undefined', function () {
            assert(mathEnforcer.subtractTen(undefined) === undefined);
        });
        it('should return undefined with an null', function () {
            assert(mathEnforcer.subtractTen(null) === undefined);
        });


        it('Should return correct num for correct input', function () {
            assert(mathEnforcer.subtractTen(5) === -5);
        });
        it('Should return correct num for correct negative input', function () {
            assert(mathEnforcer.subtractTen(-5) === -15);
        });
        it('Should return correct num for decimal input', function () {
            assert(mathEnforcer.subtractTen(15.5) === 5.5);
        });
    });
    describe('sum', function () {

        it('Should return incorrect sum for wrong input(string, number)', function () {
            assert(mathEnforcer.sum('1', 2) === undefined);
        });
        it('Should return correct sum for correct input(number, string)', function () {
            assert(mathEnforcer.sum(1, '2') === undefined);
        });
        it('Should return correct sum for correct input', function () {
            assert(mathEnforcer.sum('1', '2') === undefined);
        });


        it('Should return correct sum for correct input', function () {
            assert(mathEnforcer.sum(1, 2) === 3);
        });
        it('Should return correct sum for correct input as negative', function () {
            assert(mathEnforcer.sum(-1, -2) === -3);
        });
        it('Should return correct sum for correct decimal input', function () {
            assert(mathEnforcer.sum(1.5, 2.5) === 4);
        });
    });
});