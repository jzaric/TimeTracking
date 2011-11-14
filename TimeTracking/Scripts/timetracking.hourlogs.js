/// <reference path="jquery-1.5.1-vsdoc.js" />

$(function () {
    $('.add_hour_log').live('click', function () {
        var employee = $(this).closest('.employee');
        var hourLogs = $('.hour_logs', employee);
        var employeeId = $(employee).attr('employee_id');
        var shift = $(this).attr('shift');
        var day = $(employee).attr('day');
        $.get(servicesRoot + "HourLogs/Add", { EmployeeId: employeeId, DateString: day, Shift: shift }, function (data) {
            var item = $(data).appendTo(hourLogs)[0];
            renderSlider(item);
            calculateHours(employee);
            if (typeof calculateTotals !== 'undefined') {
                calculateTotals();
            }
        });
    });

    $('.remove_hour_log').live('click', function () {
        var hourLog = $(this).closest('.hour_log');
        var hourLogId = $(hourLog).attr('key');
        $.get(servicesRoot + "HourLogs/Remove", { HourLogId: hourLogId }, function (data) {
            if (data && data.success) {
                $(hourLog).slideUp('fast', function () {
                    var employee = $(hourLog).closest('.employee');
                    $(hourLog).empty().remove();
                    calculateHours(employee);
                    if (typeof calculateTotals !== 'undefined') {
                        calculateTotals();
                    }
                });
            }
        });
    });

    $('.slider').each(function (i, item) {
        $(item).slider({
            range: true,
            min: 0,
            max: 24,
            values: [$(item).attr('start'), $(item).attr('end')],
            step: 0.5,
            change: updateHours
        });
    });

    for (var i = 0; i < 24.5; i = i + 0.5) {
        var span = $('<span/>').text(i % 1 == 0 ? i.toString() : '.');
        $(span).css({ position: 'absolute', display: 'block', width: '1.2em', marginLeft: '-0.6em', textAlign: 'center' });
        var sliderSteps = $('.slider_steps');
        $(span).appendTo(sliderSteps).css({ left: 100 / 24 * i + '%' });
    }

    $('.employee').each(function (i, item) {
        calculateHours(item);
    });
});

function calculateHours(item) {
    var night = 0;
    var day = 0;
    $('.slider', item).each(function (i1, item1) {
        var start = Number($(item1).attr('start'));
        var end = Number($(item1).attr('end'));
        var total = end - start;
        var currDayHours = end > 6 ? (end > 22 ? 22 : end) - (start < 6 ? 6 : start > 22 ? 22 : start) : 0;
        var currNightHours = total - currDayHours;
        day += currDayHours; // (end > 22 ? 22 : end) - (start < 6 ? 6 : start);
        night += currNightHours; //(end - 22 > 0 ? end - 22 : 0) + (6 - start > 0 ? 6 - start : 0);
    });
    $('.hours > .night', item).text(night);
    $('.hours > .day', item).text(day);
}

function renderSlider(item) {
    var slider = $('.slider', item);
    $(slider).slider({
        range: true,
        min: 0,
        max: 24,
        values: [$(slider).attr('start'), $(slider).attr('end')],
        step: 0.5,
        change: updateHours
    });

    for (var i = 0; i < 24.5; i = i + 0.5) {
        var span = $('<span/>').text(i % 1 == 0 ? i.toString() : '.');
        $(span).css({ position: 'absolute', display: 'block', width: '1.2em', marginLeft: '-0.6em', textAlign: 'center' });
        var sliderSteps = $('.slider_steps', item);
        $(span).appendTo(sliderSteps).css({ left: 100 / 24 * i + '%' });
    }

    $(item).slideUp(0).slideDown('fast');
}

function updateHours(event, ui) {
    var hourLog = $(event.target).closest('.hour_log');
    var hourLogId = $(hourLog).attr('key');
    var startHour = ui.values[0];
    var endHour = ui.values[1];
    $.get(servicesRoot + "HourLogs/Update", { HourLogId: hourLogId, StartHour: startHour, EndHour: endHour }, function (data) {
        if (data && data.success) {
            $(event.target).attr('start', startHour).attr('end', endHour);
            calculateHours($(event.target).closest('.employee'));
            if (typeof calculateTotals !== 'undefined') {
                calculateTotals();
            }
        }
    });
}